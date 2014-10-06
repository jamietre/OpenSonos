﻿using System;
using NUnit.Framework;
using OpenSonos.LocalMusicServer.Browsing;

namespace OpenSonos.LocalMusicServer.Test.Unit.Browsing
{
    [TestFixture]
    public class SonosIdentifierTests
    {
        private string _uncompressedId;
        private string _compressedId;

        [SetUp]
        public void SetUp()
        {
            _uncompressedId = "\\\\some\\smb\\path";
            _compressedId = "DwAAAB+LCAAAAAAABACLiSnOz02NKc5NiilILMkAAAUyIC4PAAAA";
        }

        [Test]
        public void Ctor_WithPath_IdAndPathSetCorrectly()
        {
            var identifier = new SonosIdentifier(_uncompressedId);

            Assert.That(identifier.Id, Is.EqualTo(_compressedId));
        }

        [Test]
        public void Ctor_WithPathTooLongToBeRepresented_ThrowsException()
        {
            var ex =
                Assert.Throws<Exception>(
                    () => new SonosIdentifier("\\\\smb\\really\\very\\stupidly\\special\\and\\really\\long\\long path\\that\\will\\totally\\exceed\\the compressed one hundred and twenty eight character limit"));

            Assert.That(ex.Message, Is.EqualTo("Compressed path is too for Sonos systems."));
        }

        [Test]
        public void Ctor_WithMaxWindowsPathLength_CanCreateCompressedId()
        {
            var identifier = new SonosIdentifier("\\abcdefghjklmnopqrstuvwxyz\\ABCDEFGHIJKLMNOPQRSTUVWXYZ\\AbCdEfGhIjLlMnoPqRsTuVwZyZ\\the quick brown fox jumped ov\\er the lazy dog and this i\\s strin  a long string th\\at should be very hard toc\\ompress reliably for gzip \\compression to handle well and q");

            Assert.That(identifier.Id, Is.Not.Null);
        }

        [Test]
        public void Ctor_WhenPathHasLotsOfEntropy_CanCreateCompressedId()
        {
            var identifier = new SonosIdentifier("Justin Timberlake - The 20-20 Experience (Deluxe Edition) 2013 Pop 320kbps CBR MP3 [VX]");

            Assert.That(identifier.Id, Is.Not.Null);
        }

        [Test]
        public void FromRequestId_WithValidCompressedId_PathIsCorrect()
        {
            var identifier = SonosIdentifier.FromRequestId(_compressedId);

            Assert.That(identifier.Path, Is.EqualTo(_uncompressedId));
        }

        [Test]
        public void FromRequestId_WithNullEmptyId_PathAndIdAreEmpty()
        {
            var identifier = SonosIdentifier.FromRequestId("");

            Assert.That(identifier.Path, Is.EqualTo(""));
            Assert.That(identifier.Id, Is.EqualTo("AAAAAA=="));
        }

        [Test]
        public void FromRequestId_WithRootSonosValueProvided_PathAndIdAreEmpty()
        {
            var identifier = SonosIdentifier.FromRequestId("root");

            Assert.That(identifier.Path, Is.EqualTo(""));
            Assert.That(identifier.Id, Is.EqualTo("AAAAAA=="));
        }

        [Test]
        public void FromRequestId_WithDirectoryPath_IsDirectoryIsTrue()
        {
            var identifier = SonosIdentifier.FromRequestId(_compressedId);

            Assert.That(identifier.IsDirectory, Is.EqualTo(true));
        }
    }
}